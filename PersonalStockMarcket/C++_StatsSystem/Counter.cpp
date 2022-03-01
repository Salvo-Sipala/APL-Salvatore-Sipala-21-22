// Template for Counter class 


template <typename CountedType>
class Counter {
private:
    static size_t count;    // number of existing objects

protected:
    // default constructor
    Counter() {
        ++count;
    }

    // copy constructor
    Counter(Counter<CountedType> const&) {
        ++count;
    }

    // destructor
    ~Counter() {
        --count;
    }

public:
    // return number of existing objects:
    static size_t getCount() {
        return count;
    }
};

// initialize counter with zero
template <typename CountedType>
size_t Counter<CountedType>::count = 0;
