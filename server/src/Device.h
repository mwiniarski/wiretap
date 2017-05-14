#ifndef DEVICE_H_
#define DEVICE_H_

#include "Serializer.h"

#include <memory>
#include <cstring>

class Device
{
public:
    Device(int socket_, sockaddr_in a_);

    void operator()();
private:

    Serializer serializer;
};

#endif // DEVICE_H_
