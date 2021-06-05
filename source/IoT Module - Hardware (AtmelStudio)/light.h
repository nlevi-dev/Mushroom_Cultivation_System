/*
 * light.h
 *
 * Created: 02.05.2021 08:24:03
 *  Author: natalimj
 */ 

#include "tsl2591.h"
#ifndef LIGHT_H_
#define LIGHT_H_

void lightTask(void* pvParameters);
void lightCallback(tsl2591_returnCode_t rc);

#endif /* LIGHT_H_ */
