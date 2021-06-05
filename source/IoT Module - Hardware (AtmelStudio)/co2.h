/*
 * co2.h
 *
 * Created: 02.05.2021 08:16:48
 *  Author: popad
 */ 

#include <stdint.h>
#ifndef CO2_H_
#define CO2_H_

void co2Task(void *pvParameters);
void co2Callback(uint16_t ppm);

#endif /* CO2_H_ */