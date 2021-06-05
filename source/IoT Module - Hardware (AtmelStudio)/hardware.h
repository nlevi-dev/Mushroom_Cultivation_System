/*
 * hardware.h
 *
 * Created: 02.05.2021 07:57:51
 *  Author: popad
 */ 

#include <stdint.h>
#include "semphr.h"
#include "message_buffer.h"

#ifndef HARDWARE_H_
#define HARDWARE_H_

SemaphoreHandle_t hardware_semaphore;
MessageBufferHandle_t downlink_buffer;

typedef struct hardware_data{
	int16_t temperature;
	uint16_t humidity;
	uint16_t co2;
	uint16_t light;
}hardware_data;

hardware_data entry_data;

typedef struct DesiredData{
	int16_t desired_temp;
	uint16_t desired_hum;
	uint16_t desired_co2;
	uint16_t desired_light;
}DesiredData;

DesiredData desired_data;

#endif /* HARDWARE_H_ */