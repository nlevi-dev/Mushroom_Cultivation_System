/*
* main.c
* Author : popad & natalimj & mihai
*
*/

#include <stdio.h>
#include <avr/io.h>

#include <ATMEGA_FreeRTOS.h>
#include <task.h>
#include <semphr.h>

#include <stdio_driver.h>
#include <serial.h>

 // Needed for LoRaWAN
#include <lora_driver.h>
#include <status_leds.h>
#include "loraHandler.h"

#include "hardware.h"

#include "hih8120.h"
#include "mh_z19.h"
#include "tsl2591.h"
#include "rc_servo.h"

#include "co2.h"
#include "light.h"
#include "tempAndHum.h"
#include "motor.h"


// Prototype for LoRaWAN handler
//void lora_handler_initialise(UBaseType_t lora_handler_task_priority);


/*-----------------------------------------------------------*/
void initialiseSystem()
{
	// Set output ports for leds used in the example
	DDRA |= _BV(DDA0) | _BV(DDA7);

	// Make it possible to use stdio on COM port 0 (USB) on Arduino board - Setting 57600,8,N,1
	stdio_initialise(ser_USART0);
	
	// Status Leds driver
	status_leds_initialise(5); // Priority 5 for internal task

	downlink_buffer = xMessageBufferCreate(sizeof(lora_driver_payload_t)*2);
	lora_driver_initialise(ser_USART1, downlink_buffer);
	//lora_handler_initialise(3);
	
	entry_data.co2 =0;
	entry_data.humidity=0;
	entry_data.light=0;
	entry_data.temperature=0;
	
	//temp&hum
	int returnCode = hih8120_initialise();
	if(HIH8120_OK!=returnCode){
		printf("HIH8120 initialize error %d \n",returnCode);
		}
		else {
		puts("Humidity and Temperature driver initialized");
	}
	
	//co2
	mh_z19_initialise(ser_USART3);
	mh_z19_injectCallBack(co2Callback);
	puts("Co2 driver started \n");
	
	//light 
	returnCode = tsl2591_initialise(lightCallback);

	if (returnCode != TSL2591_OK) {
		printf("TSL2591 initialize error %d \n", returnCode);
	}
	else {
		puts("Light driver initialized");
	}

	//enable light sensor
	returnCode = tsl2591_enable();
	if (returnCode != TSL2591_OK) {
		printf("Failed to enable light sensor %d\n", returnCode);
	}
	
	//initialise servo
	rc_servo_initialise();

}

/*-----------------------------------------------------------*/
int main(void)
{
	initialiseSystem(); 
	
	
	xTaskCreate(lora_handler_task,  "Lora task",  configMINIMAL_STACK_SIZE+200, NULL, 3 , NULL );
	xTaskCreate(tempAndHumidityTask, "temp&hum task", configMINIMAL_STACK_SIZE, NULL, 1, NULL);
	xTaskCreate(co2Task, "co2 task", configMINIMAL_STACK_SIZE, NULL,1,NULL);
	xTaskCreate(lightTask,"light task",configMINIMAL_STACK_SIZE,NULL,1,NULL);
	xTaskCreate(humMotorTask, "Humidity motor Task", configMINIMAL_STACK_SIZE + 200, NULL, 2, NULL);
	
	hardware_semaphore = xSemaphoreCreateMutex();
	if((hardware_semaphore)!=NULL){
		xSemaphoreGive((hardware_semaphore));
	}
	
	printf("Program Started!!\n");
	vTaskStartScheduler(); 
	
	while (1)
	{
		;
	}
}

