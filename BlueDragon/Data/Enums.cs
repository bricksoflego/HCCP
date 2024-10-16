namespace BlueDragon.Data;
enum LocationUsed
{
    OfficeSupplies,
    Office,
    MediaCabinet,
    FamilyRoom,
    PinkRoom,
    NathanRoom,
    NathanBath,
    Garage,
    PianoStudio,
    MusicCloset,
    MasterBedroom,
    MasterBath,
    Kitchen,
    NoahRoom,
    NoahBath,
    LaundryRoom,
    Other
}

// BARCODE 93 or QR NUMBER LOOKUP
// EXAMPLE: 2202401300512 = Cable | January 30, 2024 | USB | 6.0'

enum BarcodePrefix
{
    Hardware,
    Cable,
    Component,
    Periphereal
}

enum BarcodeSuffix
{
    // HARDWARE
    Desktop=00,
    Laptop=01,
    ThinClient=02,
    MicroPC=03,
    SBC=04,
    // CABLES
    USB=05,
    HDMI=06,
    DVI=07,
    VGA=08,
    DP=09,
    Ethernet,
    Power,
    PowerAdapter,
    Video,
    Audio,
    Phone,
    Firewire,
    Coaxial,
    MIDI,
    Data,
    // COMPONENTS
    CPU,
    Fan,
    Memory,
    VideoCard,
    AudioCard,
    HDD,
    SSD,
    Motherboard,
    PowerSupply,
    // PERIPHERALS
    Monitor,
    Printer,
    Scanner,
    Keyboard,
    Mouse,
    Camera,
    KVM,
    USBHub,
    NetworkHub,
    KM,
    MusicKeyboard,
    ZIPDrive,
    // OTHER GENERAL
    MULTI,
    COMBO
}

enum BarcodePlus
{
    NA=00,
    Length05=01,
    Length10=02,
    Length15=03,
    Length20=04,
    Length25=05,
    Length30=06,
    Length35=07,
    Length40=08,
    Length45=09,
    Length50,
    Length55,
    Length60,
    Length80,
    Length90,
    Length100,
    Length120,
    Length150,
    Length200,
    Length250,
    Length300,
    Length500,
    Length1000
}