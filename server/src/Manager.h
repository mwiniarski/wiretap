#ifndef MANAGER_H_
#define MANAGER_H_

#include <stdexcept>
#include <netinet/ip.h>
#include <arpa/inet.h>
#include <string>
#include <iostream>
#include <thread>
#include <vector>
#include "Device.h"

class Manager
{
public:
    Manager(short recvPort_, short sendPort_);

    void run();

    enum M {
        RECV = 0, SEND = 1, SIZE = 2
    };
private:
    void acceptIncomingConnections(int i);
    void passControl();
    void throwError(std::string msg);

    short port[SIZE];
    int serverSocket[SIZE];
    std::vector<std::unique_ptr<Device>> devices;
};

#endif //MANAGER_H_
