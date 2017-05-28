#include "DbManager.h"

DbManager::DbManager()
{}

std::string DbManager::generateName(int len) {
    std::string const default_chars =
    "abcdefghijklmnaoqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    std::mt19937_64 gen { std::random_device()() };
    std::uniform_int_distribution<size_t> dist { 0, default_chars.length()-1 };
    std::string ret;
    std::generate_n(std::back_inserter(ret), len, [&] { return default_chars[dist(gen)]; });
    return ret;
}

std::string DbManager::addFile(int id, std::string file, int fileType)
{
    std::string type = fileType == 1 ? "P" : deviceType == "mobile" ? "A" : "T";

    std::string sql = "INSERT INTO blog_file "
                      "VALUES (DEFAULT, '" +       //id
                      file + "', "                 //path
                      "now(), '" +                 //timestamp
                      type + "', " +          //file_type
                      std::to_string(id) + ")";    //source_id

    pqxx::work work(*connection);
    work.exec(sql);
    work.commit();

    return type;
}

int DbManager::isAccepted(std::string newUuid)
{
    char type = newUuid[0];
    std::string uuid = newUuid.substr(1);

    std::string sql = "SELECT * FROM blog_device "
                      "WHERE uuid = '" + uuid + "'";

    pqxx::work work(*connection);
    pqxx::result query = work.exec(sql);

    //record exists
    if(query.size() == 1) {
        std::string status = query.front()[3].as<std::string>();
        deviceType = query.front()[2].as<std::string>();
        if(status == "active")
            return query.front()[0].as<int>();

        std::cout << "Device " << uuid << " not accepted!" << std::endl;
        return 0;
    }

    std::cout << "New device: " << uuid << std::endl;

    //new device
    if(type == 'a')
        deviceType = "mobile";
    else if (type == 'w')
        deviceType = "windows";
    else
        throwError("wrong device type");

    sql = "INSERT INTO blog_device "
          "VALUES (DEFAULT, '" +         //id
          uuid + "', '" +      //uuid
          deviceType +                   //device_type
          "', 'new', '" +                //status
          generateName(16) +             //name
          "', 10, 10)";                  //send_cycles

    work.exec(sql);
    work.commit();

    return 0;
}

void DbManager::readConfigs(){
    std::ifstream conf("../web/config.py");
    std::string line;

    for(int i=0; std::getline(conf, line) && i < 5;) {
        if(line[0] == '#')
            continue;

        int pos = line.find("'");
        int end = line.find_last_of("'");
        configs[i] = line.substr(pos+1,end-pos-1);
        i++;
    }
}

bool DbManager::connect() {
    readConfigs();

    connection = new pqxx::connection
                    (
                        "dbname = " + configs[0] +
                        " user = " + configs[1] +
                        " password = " + configs[2] +
                        " hostaddr = " + configs[3] +
                        " port = " + configs[4]
                    );

    return connection->is_open();
}

void DbManager::throwError(std::string msg) {
    throw std::logic_error("Database error on: " + msg );
}

void DbManager::disconnect()
{
    if(connection->is_open())
        connection->disconnect();
}
