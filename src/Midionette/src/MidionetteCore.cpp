﻿/*
 *  Created on: 2018/07/23
 *      Author: take
 */

#include "MidionetteCore.h"

#include <Windows.h>

namespace Elgraiv {
namespace Midionette {
namespace Unmanaged {

class MidiInputCore::MidiInputCoreImpl{
private:
	static void CALLBACK MidiInProc(
		HMIDIIN   hMidiIn,
		UINT      wMsg,
		DWORD_PTR dwInstance,
		DWORD_PTR dwParam1,
		DWORD_PTR dwParam2
	) {
		auto instance = reinterpret_cast<MidiInputCoreImpl*>(dwInstance);
		instance->ProcImpl(wMsg, dwParam1, dwParam2);
	}

	void ProcImpl(
		UINT      wMsg,
		DWORD_PTR dwParam1,
		DWORD_PTR dwParam2) {
		switch (wMsg)
		{
		case MIM_OPEN:
			m_functions.opened();
			break;
		case MIM_CLOSE:
			m_functions.closed();
			break;
		case MIM_DATA:
		{
			auto status = static_cast<uint8_t>(dwParam1 & 0xff);
			auto data0 = static_cast<uint8_t>((dwParam1 >> 8) & 0xff);
			auto data1 = static_cast<uint8_t>((dwParam1 >> 16) & 0xff);
			m_functions.dataReceived(status, data0, data1, static_cast<uint32_t>(dwParam2));
		}
			break;
		default:
			break;
		}
	}

	HMIDIIN m_handle;
	MidiCallbackFunctions m_functions;

public:

	MidiInputCoreImpl() {

	}
	~MidiInputCoreImpl() {
		midiInStop(m_handle);
		midiInClose(m_handle);
	}

	bool Initialize(uint32_t deviceId) {
		auto resultCode = midiInOpen(
			&m_handle,
			deviceId,
			reinterpret_cast<DWORD_PTR>(&MidiInProc),
			reinterpret_cast<DWORD_PTR>(this),
			CALLBACK_FUNCTION);
		if (resultCode != MMSYSERR_NOERROR) {
			return false;
		}
		resultCode = midiInStart(m_handle);
		return resultCode == MMSYSERR_NOERROR;
	}

	void SetCallback(MidiCallbackFunctions & functions)
	{
		m_functions = functions;
	}

};

MidiInputCore::MidiInputCore():m_pImpl(std::make_unique<MidiInputCoreImpl>())
{
}

MidiInputCore::~MidiInputCore()
{
}

void MidiInputCore::SetCallback(MidiCallbackFunctions & functions)
{
	m_pImpl->SetCallback(functions);
}

bool MidiInputCore::Initialize(uint32_t deviceId)
{
	return m_pImpl->Initialize(deviceId);
}

uint32_t MidiInputCore::GetNumDevices()
{
	return midiInGetNumDevs();
}

void MidiInputCore::GetDevices(std::vector<MidiDevice>& devices)
{
	auto num = midiInGetNumDevs();
	for (auto i = 0U; i < num; i++) {
		MIDIINCAPS info;
		auto result=midiInGetDevCaps(i, &info, sizeof(info));
		if (result == MMSYSERR_NOERROR) {
			MidiDevice device;
			device.name = std::wstring(info.szPname);
			device.id = i;
			devices.emplace_back(device);
		}
	}

}

}  // namespace Unmanaged
}  // namespace Midionette
}  // namespace Elgraiv

