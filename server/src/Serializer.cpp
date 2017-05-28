#include "Serializer.h"

Serializer::Serializer(int socket_, sockaddr_in a_)
    :connection(socket_, a_)
{}

void Serializer::getFrame(Frame frame)
{
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

    sendFrame(Frame::ACK);
}

void Serializer::sendFrame(Frame frame)
{
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

std::string Serializer::acceptDevice()
{
    getFrame(Frame::TRANSFER);
    if(transfer.type != Transfer::Type::REGISTER)
        throwError("accept device");

    getFrame(Frame::PACKET);
    return std::string(packet.data);
}

int Serializer::getMessage(FILE * fp, std::string path, std::string file)
{
    //wait for first frame to start connection
    connection.setTimeout(0);
    getFrame(Frame::TRANSFER);

    std::cout<< transfer.type << " " << transfer.packetCount << " " << (int)transfer.lastPacketSize<< "\n";

    std::string fullPath = path + file;

    fp = fopen(fullPath.c_str(), "wb");

    if(fp == nullptr) {
        std::string cmd = "mkdir -p " + path;
        system(cmd.c_str());
        fp = fopen(fullPath.c_str(), "wb");
    }

    //get the rest of the file
    connection.setTimeout(5);
    for(short i = 1; i <= transfer.packetCount - 1; i++) {

        getFrame(Frame::PACKET);
        fwrite(packet.data, 1, 256, fp);
    }

    //get last packet
    getFrame(Frame::PACKET);

    fwrite(packet.data, 1, transfer.lastPacketSize, fp);
    fclose(fp);


    if(transfer.type == Transfer::Type::DATA_TYPE1)
        return 1;
    else
        return 2;
}

void Serializer::throwError(std::string msg) {
    throw std::logic_error("Serialization error on: " + msg );
}
