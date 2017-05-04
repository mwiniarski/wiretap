#ifndef MANAGER_H_
#define MANAGER_H_

#include <stdexcept>
#include <netinet/ip.h>
#include <string>
#include <thread>
#include "Device.h"

class Manager
{
public:
    Manager(short int port);

    void run();

private:
    short int serverPort;
    int serverSocket;
    sockaddr_in serverAddress;
    void acceptIncomingConnections();

    void throwError(std::string msg);
};

#endif //MANAGER_H_
