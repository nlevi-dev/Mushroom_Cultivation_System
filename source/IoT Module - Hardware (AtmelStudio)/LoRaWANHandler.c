/*
* loraWANHandler.c
*
*  Author: popad & mihai & natalimj
*/
#include <stddef.h>
#include <stdio.h>

#include <ATMEGA_FreeRTOS.h>

#include <lora_driver.h>
#include <status_leds.h>

#include "semphr.h"
#include "hardware.h"
#include "loraHandler.h"


// Parameters for OTAA join 
#define LORA_appEUI "080AE22D17745AEA"
#define LORA_appKEY "12A67C3072B659179BC2216FE32B7DC9"

static lora_driver_payload_t _uplink_payload;
static lora_driver_payload_t _downlink_payload;

static void _lora_setup(void)
{
	char _out_buf[20];
	lora_driver_returnCode_t rc;
	status_leds_slowBlink(led_ST2); 


	// Factory reset the transceiver
	printf("FactoryReset >%s<\n", lora_driver_mapReturnCodeToText(lora_driver_rn2483FactoryReset()));
	
	// Configure to EU868 LoRaWAN standards
	printf("Configure to EU868 >%s<\n", lora_driver_mapReturnCodeToText(lora_driver_configureToEu868()));

	// Get the transceivers HW EUI
	rc = lora_driver_getRn2483Hweui(_out_buf);
	printf("Get HWEUI >%s<: %s\n",lora_driver_mapReturnCodeToText(rc), _out_buf);

	// Set the HWEUI as DevEUI in the LoRaWAN software stack in the transceiver
	printf("Set DevEUI: %s >%s<\n", _out_buf, lora_driver_mapReturnCodeToText(lora_driver_setDeviceIdentifier(_out_buf)));

	// Set Over The Air Activation parameters to be ready to join the LoRaWAN
	printf("Set OTAA Identity appEUI:%s appKEY:%s devEUI:%s >%s<\n", LORA_appEUI, LORA_appKEY, _out_buf, lora_driver_mapReturnCodeToText(lora_driver_setOtaaIdentity(LORA_appEUI,LORA_appKEY,_out_buf)));

	// Save all the MAC settings in the transceiver
	printf("Save mac >%s<\n",lora_driver_mapReturnCodeToText(lora_driver_saveMac()));

	// Enable Adaptive Data Rate
	printf("Set Adaptive Data Rate: ON >%s<\n", lora_driver_mapReturnCodeToText(lora_driver_setAdaptiveDataRate(LORA_ON)));

	// Set receiver window1 delay to 500 ms - this is needed if down-link messages will be used
	printf("Set Receiver Delay: %d ms >%s<\n", 500, lora_driver_mapReturnCodeToText(lora_driver_setReceiveDelay(500)));



	// Join the LoRaWAN
	uint8_t maxJoinTriesLeft = 10;
	
	do {
		rc = lora_driver_join(LORA_OTAA);
		//printf("Join Network TriesLeft:%d >%s<\n", maxJoinTriesLeft, lora_driver_mapReturnCodeToText(rc));

		if ( rc != LORA_ACCEPTED)
		{
			// Wait 5 sec and lets try again
			vTaskDelay(pdMS_TO_TICKS(5000UL));
		}
		else
		{
			break;
		}
	} while (--maxJoinTriesLeft);

	if (rc == LORA_ACCEPTED)
	{
		
		//puts("Connection succeeded \n");
	}
	else
	{
		//puts("Connection failed \n");
		while (1)
		{
			taskYIELD();
		}
	}
}

/*-----------------------------------------------------------*/
void lora_handler_task( void *pvParameters )
{
	// Hardware reset of LoRaWAN transceiver
	lora_driver_resetRn2483(1);
	vTaskDelay(2);
	lora_driver_resetRn2483(0);
	// Give it a chance to wakeup
	vTaskDelay(150);

	lora_driver_flushBuffers(); // get rid of first version string from module after reset!

	_lora_setup();

	_uplink_payload.len = 8;
	_uplink_payload.portNo = 1; 
	
	_downlink_payload.len = 8;
	_downlink_payload.portNo = 1;
	
	for(;;)
	{
		vTaskDelay(10000); //500 = aprox 30 sec (10000 - aprox 3.5 min)
		xSemaphoreTake(hardware_semaphore,portMAX_DELAY);
		
		_uplink_payload.bytes[0] = entry_data.humidity >> 8;
		_uplink_payload.bytes[1] = entry_data.humidity & 0xFF;
		_uplink_payload.bytes[2] = entry_data.temperature >> 8;
		_uplink_payload.bytes[3] = entry_data.temperature & 0xFF;
		_uplink_payload.bytes[4] = entry_data.co2 >> 8;
		_uplink_payload.bytes[5] = entry_data.co2 & 0xFF;
		_uplink_payload.bytes[6] = entry_data.light >> 8;
		_uplink_payload.bytes[7] = entry_data.light & 0xFF;
		
		lora_driver_returnCode_t rc;
		
		rc = lora_driver_sendUploadMessage(false, &_uplink_payload);
		
		if (rc  == LORA_MAC_TX_OK )
		{
			//puts("MESSAGE SENT \n");
			// The uplink message is sent and there is no downlink message received
		}
		else if(rc==LORA_MAC_RX)
		{
			// The uplink message is sent and a downlink message is received
			//puts("MESSAGE SENT \n");
			
			xMessageBufferReceive(downlink_buffer, &_downlink_payload, sizeof(lora_driver_payload_t),portMAX_DELAY);
			//printf("DOWN LINK: from port: %d with %d bytes received! \n", _downlink_payload.portNo, _downlink_payload.len);
			
			
			if(_downlink_payload.len==8) //number of bytes we send and expect to receive
			{
				desired_data.desired_temp=(_downlink_payload.bytes[0] << 8) + _downlink_payload.bytes[1];
				desired_data.desired_hum=(_downlink_payload.bytes[2] << 8) + _downlink_payload.bytes[3];
				desired_data.desired_co2=(_downlink_payload.bytes[4] << 8) + _downlink_payload.bytes[5];
				desired_data.desired_light=(_downlink_payload.bytes[6] << 8) + _downlink_payload.bytes[7];
				//printf("values received: %d, %d, %d, %d \n \n",desired_data.desired_temp,desired_data.desired_hum,desired_data.desired_co2,desired_data.desired_light);
			}	
		}
				
		xSemaphoreGive(hardware_semaphore);

	}
}

