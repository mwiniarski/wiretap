#include "Device.h"

Device::Device(int socket_, sockaddr_in a_)
    :connection(socket_, a_)
{}

void Device::operator()(){
    const int BUFFER_SIZE = 256;

    char buffer[BUFFER_SIZE];
    while(true) {
        memset(buffer, 0, BUFFER_SIZE);
        int messageSize;
        // read
        try{
            messageSize = connection.getMessage(buffer, BUFFER_SIZE);
        } catch(std::exception& e) {
            break;
        }

        // write
        connection.sendMessage(buffer, messageSize);

    }


}
