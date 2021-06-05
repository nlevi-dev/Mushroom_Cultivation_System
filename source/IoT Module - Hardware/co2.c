/*
 * co2.c
 *
 * Created: 02.05.2021 08:16:31
 *  Author: popad
 */ 

#include <ATMEGA_FreeRTOS.h>
#include <stdio.h>
#include <stdio_driver.h>
#include "co2.h"
#include "hardware.h"
#include "semphr.h"
#include "mh_z19.h"

void co2Task(void *pvParameters){
	(void)pvParameters;
	while(1){
		vTaskDelay(50);
		int statusCode = mh_z19_takeMeassuring();
		if(statusCode!=MHZ19_OK){
			printf("CO2 measuring error %d\n",statusCode);
		}
		vTaskDelay(9900);
	}
}
void co2Callback(uint16_t ppm){
	xSemaphoreTake(hardware_semaphore,portMAX_DELAY);
	entry_data.co2=ppm;
	//printf("CO2 VALUE : %d ", entry_data.co2);
	xSemaphoreGive(hardware_semaphore);
}
