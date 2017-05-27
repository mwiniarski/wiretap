#ifndef DBMANAGER_H_
#define DBMANAGER_H_
#include <pqxx/pqxx>
#include <random>
#include <algorithm>
#include <string>
#include <iostream>
#include <fstream>

class DbManager {
public:
    DbManager();
    ~DbManager(){delete connection;}

    int isAccepted(int uuid, std::string s);
    void addFile(int uuid, std::string s);
    bool connect();
    void disconnect();
private:
    void readConfigs();
    std::string generateName(int uuid);
    pqxx::connection * connection;
    std::string configs[5];
};

#endif
