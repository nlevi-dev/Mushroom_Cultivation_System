/*
 * light.c
 *
 * Created: 02.05.2021 08:24:13
 *  Author: natalimj
 */ 

#include <ATMEGA_FreeRTOS.h>
#include <stdio.h>
#include <stdio_driver.h>
#include "hardware.h"
#include "light.h"
#include "tsl2591.h"
#include "semphr.h"
#include "rc_servo.h"

void lightTask(void* pvParameters) {
	(void)pvParameters;

	while (1) {
		vTaskDelay(70);
		int statusCode = tsl2591_fetchData();
		if (statusCode != TSL2591_OK) {
			printf("Light data error: %d\n", statusCode);
		} 
		vTaskDelay(9900);
	}

}


void lightCallback(tsl2591_returnCode_t rc) {
	float lux;
	xSemaphoreTake(hardware_semaphore, portMAX_DELAY);
	
	if (TSL2591_OK == (rc = tsl2591_getLux(&lux)))
	{
	
		entry_data.light = (uint16_t)lux;
	}
	else if (TSL2591_OVERFLOW == rc)
	{
		printf("Lux overflow\n");
	}
	if(desired_data.desired_light>entry_data.light){
		rc_servo_setPosition(1,100)	;
		//printf("Motor is moving right\nLight level is turned up");

	}
	if(desired_data.desired_light<entry_data.light){
		rc_servo_setPosition(1,-100)	;
		//printf("Motor is moving left\nLight level is turned down");
		
	}
	
	xSemaphoreGive(hardware_semaphore);
}
