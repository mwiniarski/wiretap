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

void DbManager::addFile(int id, std::string file)
{
    std::string sql = "INSERT INTO blog_file "
                      "VALUES (DEFAULT, '" +        //id
                      file + "', "                  //path
                      "now(), 'P', " +             //timestamp, file_type
                      std::to_string(id) + ")";    //source_id

    pqxx::work work(*connection);
    work.exec(sql);
    work.commit();
}

int DbManager::isAccepted(int uuid, std::string deviceType)
{
    std::string sql = "SELECT * FROM blog_device "
                      "WHERE uuid = " + std::to_string(uuid);

    pqxx::work work(*connection);
    pqxx::result query = work.exec(sql);

    //record exists
    if(query.size() == 1) {
        std::string status = query.front()[3].as<std::string>();
        if(status == "active")
            return query.front()[0].as<int>();

        return 0;
    }

    //new device
    sql = "INSERT INTO blog_device "
          "VALUES (DEFAULT, " +          //id
          std::to_string(uuid) + ", '" + //uuid
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

void DbManager::disconnect()
{
    if(connection->is_open())
        connection->disconnect();
}
