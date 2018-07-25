/*
 *  Created on: 2018/07/23
 *      Author: take
 */

#include "MidionetteCore.h"

#include <msclr/marshal_cppstd.h>

namespace Elgraiv{
namespace Midionette{
namespace Device {
namespace {
std::function<void()> CreateCallback(gcroot<System::Action^>& del) {
	return [del]() {del->Invoke(); };
}

Unmanaged::DataReceivedCallback CreateCallback(gcroot<System::Action<uint8_t, uint8_t, uint8_t, uint32_t>^>& del) {
	return [del](uint8_t status, uint8_t data0, uint8_t data1, uint32_t timestamp) {
		del->Invoke(status, data0, data1, timestamp);
	};
}

}

public value struct MidiData {
public:
	property uint8_t Status;
	property uint8_t MidiData0;
	property uint8_t MidiData1;
	property uint32_t Timestamp;
};

public value struct DeviceInfo {
	property uint32_t Id;
	property System::String^ Name;
};

public ref class MidiDataEventArgs :System::EventArgs {
private:
	MidiData _data;
public:
	property MidiData Data { MidiData get() { return _data; }};
	MidiDataEventArgs(MidiData data) {
		_data = data;
	}

};

public ref class MidiInput {
public:
	event System::EventHandler<MidiDataEventArgs^>^  MidiDataReceived;
	event System::EventHandler<System::EventArgs^>^  MidiOpened;
	event System::EventHandler<System::EventArgs^>^  MidiClosed;
private:
	Unmanaged::MidiInputCore* _core;

private:
	void OnMidiOpened() {
		MidiOpened(this, System::EventArgs::Empty);
	}
	void OnMidiClosed() {
		MidiClosed(this, System::EventArgs::Empty);
	}

	void OnMidiDataReceived(uint8_t status, uint8_t data0, uint8_t data1, uint32_t timestamp) {
		auto data = MidiData();
		data.Status = status;
		data.MidiData0 = data0;
		data.MidiData1 = data1;
		data.Timestamp = timestamp;
		auto e = gcnew MidiDataEventArgs(data);
		MidiDataReceived(this, e);
	}

public:
	MidiInput() :_core(nullptr) {
		_core = new Unmanaged::MidiInputCore();

		Unmanaged::MidiCallbackFunctions functions;
		gcroot<System::Action^> actionOpened(gcnew System::Action(this, &MidiInput::OnMidiOpened));
		functions.opened = CreateCallback(actionOpened);
		gcroot<System::Action^> actionClosed(gcnew System::Action(this, &MidiInput::OnMidiClosed));
		functions.closed = CreateCallback(actionClosed);
		gcroot<System::Action<uint8_t, uint8_t, uint8_t, uint32_t>^> actionDataReceived(gcnew System::Action<uint8_t, uint8_t, uint8_t, uint32_t>(this, &MidiInput::OnMidiDataReceived));
		functions.dataReceived = CreateCallback(actionDataReceived);
		_core->SetCallback(functions);
	}

	property System::String^ Name {
		System::String^ get() {
			return msclr::interop::marshal_as<System::String^>(_core->GetName());
		}

	}

	!MidiInput() {

	}
	~MidiInput()
	{
		delete _core;
		_core = nullptr;
		this->!MidiInput();
	}

	bool Start() {
		if (_core) {
			return _core->Start();
		}
		return false;
	}

	bool Stop() {
		if (_core) {
			return _core->Stop();
		}
		return false;
	}

	bool Initialize(System::UInt32 deviceId) {
		if (_core) {
			return _core->Initialize(deviceId);
		}
		return false;
	}

	static System::UInt32 GetNumDevices() {
		return Unmanaged::MidiInputCore::GetNumDevices();
	}

	static System::Collections::Generic::List<DeviceInfo>^ GetDevices() {
		auto list = gcnew System::Collections::Generic::List<DeviceInfo>(10);
		std::vector<Unmanaged::MidiDevice> devices;
		Unmanaged::MidiInputCore::GetDevices(devices);
		for (auto& device : devices) {
			auto managed = DeviceInfo();
			managed.Id = device.id;
			managed.Name = msclr::interop::marshal_as<System::String^>(device.name);
			list->Add(managed);
		}
		return list;
	}
};
}
}
}
