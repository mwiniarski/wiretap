#ifndef DEVICE_H_
#define DEVICE_H_

#include "Connection.h"

#include <memory>
#include <cstring>

class Device
{
public:
    Device(int socket_, sockaddr_in a_);

    void operator()();
private:
    Connection connection;
};

#endif // DEVICE_H_
