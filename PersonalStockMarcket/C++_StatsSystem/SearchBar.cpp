#include "Counter.cpp"
#include <iostream>


template <class ButtonType>
class Button : public Counter<ButtonType> {
public:
    void printNumberOfInstance() {
        std::cout << "Number of Button is: " << Button::getCount() << std::endl;
    }

};

template <class WidgetType>
class Widget : public Counter<Widget<WidgetType>> {
public:
    void printNumberOfInstance() {
        std::cout << "Number of Widget is: " << Widget::getCount() << std::endl;
    }

};

template <class SearchBarType>
class SearchBar : public Widget<SearchBarType> {
private:
    std::string symbol;

public:
    void printNumberOfInstance() {
        std::cout << "Number of SearchBar is: " << SearchBar::getCount() << std::endl;
    }
};
