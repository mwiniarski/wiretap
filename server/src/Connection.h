#ifndef CONNECTION_H_
#define CONNECTION_H_

#include <netinet/ip.h>
#include <string>
#include <stdexcept>
#include <iostream>

class Connection {
public:
    Connection(int socket_, sockaddr_in a_);

    void sendMessage(char * message, int length);
    void getMessage(char * message, int length, int timeout);

    sockaddr_in getAddress();
private:
    void throwError(std::string msg);

    sockaddr_in address;
    int socket;
    fd_set readSet, writeSet;
    timeval timeout;
};

#endif //CONNECTION_H_
