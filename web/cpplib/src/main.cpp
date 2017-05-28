#include <boost/python.hpp>
#include <iostream>
char const* greet(std::string a)
{
   return "hello, world";
}
char const* greet2(char* a)
{
  std::cout << a;

   return a;
}

BOOST_PYTHON_MODULE(hello)
{
    boost::python::def("greet", greet);
    boost::python::def("greet2", greet2);
}
