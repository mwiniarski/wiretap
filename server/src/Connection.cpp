#include "Connection.h"

Connection::Connection(int socket_, sockaddr_in a_)
    :address(a_), socket(socket_)
{}

sockaddr_in Connection::getAddress() {
    return address;
}

void Connection::setTimeout(int timeout) {
    timeval tv;
    tv.tv_sec = timeout;
    tv.tv_usec = 0;
    setsockopt(socket, SOL_SOCKET, SO_RCVTIMEO, (const char*)&tv,sizeof(struct timeval));
}

//send message to client
void Connection::sendMessage(char * message, int length){
    //select() przed sendem - check this
    send(socket, message, length, 0);
}

//get message from client - blocking!
void Connection::getMessage(char * message, int length){

    int bytes = 0;

    while(bytes != length) {
        int received = recv(socket, message, length, 0);
        bytes += received;

        //happens when client exits application or connation fails
        if (received < 0){
            throwError("receiving");
        }
    }
}

void Connection::throwError(std::string msg) {
    throw std::logic_error("Connection error on: " + msg );
}
