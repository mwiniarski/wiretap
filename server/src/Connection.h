#ifndef CONNECTION_H_
#define CONNECTION_H_

#include <netinet/ip.h>
#include <string>
#include <stdexcept>

class Connection {
public:
    Connection(int socket_, sockaddr_in a_);

    void sendMessage(char * message, int length);
    int getMessage(char * message, int length);

    sockaddr_in getAddress();
private:
    void throwError(std::string msg);
    sockaddr_in address;
    int socket;
};

#endif //CONNECTION_H_
