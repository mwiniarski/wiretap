#include "Connection.h"

Connection::Connection(int socket_, sockaddr_in a_)
    :address(a_), socket(socket_)
{
    FD_ZERO(&readSet);
    FD_ZERO(&writeSet);
    FD_SET(socket, &readSet);
    FD_SET(socket, &writeSet);
}

sockaddr_in Connection::getAddress() {
    return address;
}

//send message to client
void Connection::sendMessage(char * message, int length){
    //select() przed sendem - check this
    send(socket, message, length, 0);
}

//get message from client - blocking!
void Connection::getMessage(char * message, int length, int timeout){

    int bytes = 0;
    timeval tv;
    tv.tv_sec = timeout;
    tv.tv_uses = 0; 

    while(bytes != length) {
        int result = select(socket+1, &readSet, NULL, NULL, )
        timeout ++;
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
