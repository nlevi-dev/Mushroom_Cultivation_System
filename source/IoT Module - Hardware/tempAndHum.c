/*
 * tempAndHum.c
 *
 * Created: 02.05.2021 08:24:51
 *  Author: popad
 */ 

#include <ATMEGA_FreeRTOS.h>
#include <stdio.h>
#include "tempAndHum.h"
#include "hih8120.h"
#include <stdio_driver.h>
#include "semphr.h"
#include "hardware.h"
#include "rc_servo.h"

void tempAndHumidityTask(void* pvParameters){
	(void) pvParameters;
	
	while(1){
		vTaskDelay(50);
		
		int returnCode = hih8120_wakeup();
		if(HIH8120_OK != returnCode && returnCode!= HIH8120_TWI_BUSY){
			printf("HIH8120 wakeup error %d \n",returnCode);
		}
		
		vTaskDelay(10);
		
		returnCode = hih8120_measure();
		if(HIH8120_OK!=returnCode && returnCode!= HIH8120_TWI_BUSY){
			printf("HIH8120 measure error %d \n",returnCode);
		}
		
		vTaskDelay(15);
		
		xSemaphoreTake(hardware_semaphore, portMAX_DELAY);
		
		entry_data.humidity=hih8120_getHumidityPercent_x10();
		entry_data.temperature=hih8120_getTemperature_x10();
		//printf("Humidity= %d and Temperature= %d \n",entry_data.humidity,entry_data.temperature);
		
		xSemaphoreGive(hardware_semaphore);
		
		vTaskDelay(9900); 
	}
	
}