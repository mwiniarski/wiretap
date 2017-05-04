#include "Manager.h"

Manager::Manager(short int p_)
    :serverPort(p_)
{}

void Manager::acceptIncomingConnections() {
    int newSocket;
    sockaddr_in newAddress;
    socklen_t addrLength;


    while(true) {
        newSocket = accept(serverSocket, (sockaddr *) &newAddress, &addrLength);

        //conection is valid
        if (newSocket > 0) {

            //use of operator() in Device
            Device newDevice(newSocket, newAddress);
            std::thread newThread(newDevice);
            newThread.detach();
        }
    }
}

void Manager::run() {

    //create TCP socket
    if ((serverSocket = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        throwError("Socket");
    }

    //set port & allow any incoming address
    serverAddress.sin_family = AF_INET;
    serverAddress.sin_port = htons(serverPort);
    serverAddress.sin_addr.s_addr = INADDR_ANY;

    // reuse same port after disconnect
    int optval;
    setsockopt(serverSocket, SOL_SOCKET, SO_REUSEADDR, &optval, sizeof(optval));

    //bind socket
    if (bind(serverSocket, (sockaddr *) &serverAddress, sizeof(serverAddress)) < 0) {
        throwError("Bind");
    }

    //listen & set max connection number
    listen(serverSocket, 16);

    acceptIncomingConnections();
}

void Manager::throwError(std::string msg){
    throw std::runtime_error("Manager error on: " + msg );
}
