/*
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
			break;
		case MIM_CLOSE:
			break;
		case MIM_DATA:
			break;
		default:
			break;
		}
	}

	HMIDIIN m_handle;

public:

	MidiInputCoreImpl() {

	}
	~MidiInputCoreImpl() {

	}

	bool Initialize(uint32_t deviceId) {
		auto resultCode = midiInOpen(
			&m_handle,
			deviceId,
			reinterpret_cast<DWORD_PTR>(&MidiInProc),
			reinterpret_cast<DWORD_PTR>(this),
			CALLBACK_FUNCTION);
		return resultCode == MMSYSERR_NOERROR;
	}
};

MidiInputCore::MidiInputCore()
{
}

MidiInputCore::~MidiInputCore()
{
}

void MidiInputCore::SetCallback(MidiCallbackFunctions & functions)
{
}

}  // namespace Unmanaged
}  // namespace Midionette
}  // namespace Elgraiv

