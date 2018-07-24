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

struct MidiCallbackFunctions {
	std::function<void()> opened;
	std::function<void()> closed;
};

struct MidiDevice {
	uint32_t id;
	std::wstring name;
};

class MidiInputCore{
public:
	MidiInputCore();
	~MidiInputCore();

	void SetCallback(MidiCallbackFunctions& functions);

	static uint32_t GetNumDevices();
	static void GetDevices(std::vector<MidiDevice>& devices);
private:
	class MidiInputCoreImpl;
	std::unique_ptr<MidiInputCoreImpl> m_pImpl;
};

}  // namespace Unmanaged
}  // namespace Midionette
}  // namespace Elgraiv

