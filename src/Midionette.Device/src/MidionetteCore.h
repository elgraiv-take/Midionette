/*
 *  Created on: 2018/07/23
 *      Author: take
 */

#pragma once

#include <memory>
#include <functional>
#include <vector>
#include <string>

namespace Elgraiv {
namespace Midionette {
namespace Unmanaged {

using MidiOpenedCallback = std::function<void()>;
using MidiClosedCallback = std::function<void()>;
using DataReceivedCallback = std::function<void(uint8_t, uint8_t, uint8_t, uint32_t)>;

struct MidiCallbackFunctions {
	MidiOpenedCallback opened;
	MidiClosedCallback closed;
	DataReceivedCallback dataReceived;
};

struct MidiDevice {
	uint32_t id;
	std::wstring name;
};

class MidiInputCore{
public:
	MidiInputCore();
	~MidiInputCore();

	void SetCallback(const MidiCallbackFunctions& functions);
	bool Initialize(uint32_t deviceId);

	bool Start();
	bool Stop();

	const std::wstring& GetName()const;

	static uint32_t GetNumDevices();
	static void GetDevices(std::vector<MidiDevice>& devices);
private:
	class MidiInputCoreImpl;
	std::unique_ptr<MidiInputCoreImpl> m_pImpl;
};

}  // namespace Unmanaged
}  // namespace Midionette
}  // namespace Elgraiv

