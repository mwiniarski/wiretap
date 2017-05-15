#ifndef SERIALIZER_H_
#define SERIALIZER_H_

#include <string>
#include <stdexcept>
#include "Connection.h"

class Serializer
{
public:
    Serializer(int socket_, sockaddr_in a_);

private:
    enum class Frame {
        TRANSFER = 0, ACK = 1, PACKET = 2
    };

    struct Transfer {
        enum Type {
            REGISTER, DATA_TYPE1, DATA_TYPE2, CONFIG
        };
        Type type;
        short packetCount;
        unsigned char lastPacketSize;
    } transfer;

    struct Ack {
        bool accept;
    } ack;

    struct Packet {
        unsigned char data[256];
    } packet;

    void sendFrame(Frame frame);
    void getFrame(Frame frame);

    Connection connection;

public:
    void sendMessage();
    std::string getMessage(FILE *fp, const char* file);
};

#endif //SERIALIZER_H_
