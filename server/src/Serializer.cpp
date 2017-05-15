#include "Serializer.h"

Serializer::Serializer(int socket_, sockaddr_in a_)
    :connection(socket_, a_)
{}

void Serializer::sendMessage() {}

void Serializer::getFrame(Frame frame) {
    switch(frame) {
        case Frame::TRANSFER:
            char bufferT[4];
            connection.getMessage(bufferT, 4);

            transfer.type = static_cast<Transfer::Type>(bufferT[0]);
            transfer.packetCount = (unsigned char)bufferT[1] << 8 | (unsigned char)bufferT[2];
            transfer.lastPacketSize = (unsigned char)bufferT[3];

            break;
        case Frame::ACK:
            char bufferA[1];
            connection.getMessage(bufferA, 1);

            ack.accept = (bool)bufferA[0];
            break;
        case Frame::PACKET:
            char bufferP[256];
            connection.getMessage(bufferP, 256);

            std::copy(bufferP, bufferP + 256, packet.data);
            break;
    }
}

void Serializer::sendFrame(Frame frame) {

    switch(frame) {
        case Frame::TRANSFER:
            break;

        case Frame::ACK:
            char buffer[1];
            buffer[0] = 1;
            connection.sendMessage(buffer, 1);
            break;

        case Frame::PACKET: break;
    }
}

std::string Serializer::getMessage(FILE *fp, const char* file) {

    //wait for first frame to start connection
    connection.setTimeout(0);
    getFrame(Frame::TRANSFER);
    sendFrame(Frame::ACK);

    std::cout<<transfer.type << " " << transfer.packetCount << " " << (int)transfer.lastPacketSize<< "\n";

    fp = fopen(file, "wb");

    //get the rest of the file
    connection.setTimeout(5);
    for(short i = 1; i <= transfer.packetCount - 1; i++) {

        getFrame(Frame::PACKET);
        sendFrame(Frame::ACK);

        fwrite(packet.data, 1, 256, fp);
    }

    //get last packet
    getFrame(Frame::PACKET);
    sendFrame(Frame::ACK);

    fwrite(packet.data, 1, transfer.lastPacketSize, fp);

    std::cout << "FINISHED \n";

    //extension of received file
    return ".png";
}
