#ifndef DEVICE_H_
#define DEVICE_H_

#include "Serializer.h"
#include "DbManager.h"

#include <memory>
#include <cstring>
#include <cstdio>
#include <chrono>
#include <iostream>

class Device
{
public:
    Device(int socket_, sockaddr_in a_);

    void operator()();
private:
    std::string getTimestamp();

    int uuid, id;
    Serializer serializer;
    DbManager database;
};

#endif // DEVICE_H_
