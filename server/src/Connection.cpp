#include "Connection.h"

Connection::Connection(int socket_, sockaddr_in a_)
    :address(a_), socket(socket_)
{}

sockaddr_in Connection::getAddress() {
    return address;
}

//send message to client
void Connection::sendMessage(char * message, int length){
	send(socket, message, length, 0);
}

//get message from client - blocking!
int Connection::getMessage(char * message, int length){

	//happens when client exits application or connation fails
    int bytesReceived = recv(socket, message, length, 0);

    if (bytesReceived <= 0){
		throwError("receiving");
	}

    return bytesReceived;
}

void Connection::throwError(std::string msg) {
    throw std::runtime_error("Connection error on: " + msg );
}
