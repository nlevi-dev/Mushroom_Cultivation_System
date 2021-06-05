/*
 * motor.c
 *
 * Created: 04.05.2021 13:51:49
 *  Author: natalimj
 */ 

#include <ATMEGA_FreeRTOS.h>
#include <stdio.h>
#include <stdio_driver.h>
#include "hardware.h"
#include "motor.h"
#include "semphr.h"

void humMotorTask(void* pvParameters) {
	(void) pvParameters;
	
	while (1) {
		
		
		xSemaphoreTake(hardware_semaphore, portMAX_DELAY);
		
		if (desired_data.desired_hum > entry_data.humidity)
		{
			//printf("Water motor is moving right \n");
			rc_servo_setPosition(0,100);
			vTaskDelay(50);
			//printf("Water motor is moving left \n");
			rc_servo_setPosition(0,-100);
		}
		
		xSemaphoreGive(hardware_semaphore);
		
		vTaskDelay(9800);
	}
}

