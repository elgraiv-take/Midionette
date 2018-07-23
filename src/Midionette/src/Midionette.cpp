/*
 *  Created on: 2018/07/23
 *      Author: take
 */

#include "MidionetteCore.h"

#include <msclr/marshal_cppstd.h>

namespace Elgraiv{
namespace Midionette{

public value struct MidiData {
public:
	property uint8_t Status;
	property uint8_t MidiData0;
	property uint8_t MidiData1;
	property uint32_t Timestamp;
};

public ref class MidiDataEventArgs :System::EventArgs {
public:
	property MidiData Data;

};

public ref class MidiInput {
public:
	event System::EventHandler<MidiDataEventArgs^>^  MidiDataReceived;
private:
	Unmanaged::MidiInputCore* _core;


public:
	MidiInput() {
		_core = new Unmanaged::MidiInputCore();
	}

	!MidiInput() {

	}
	~MidiInput()
	{
		delete _core;
		_core = nullptr;
		this->!MidiInput();
	}

};

}
}
