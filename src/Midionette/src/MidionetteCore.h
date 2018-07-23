/*
 *  Created on: 2018/07/23
 *      Author: take
 */

#pragma once

#include <memory>
#include <functional>

namespace Elgraiv {
namespace Midionette {
namespace Unmanaged {

struct MidiCallbackFunctions {
	std::function<void()> opened;
	std::function<void()> closed;
};

class MidiInputCore{
public:
	MidiInputCore();
	~MidiInputCore();

	void SetCallback(MidiCallbackFunctions& functions);

private:
	class MidiInputCoreImpl;
	std::unique_ptr<MidiInputCoreImpl> m_pImpl;
};

}  // namespace Unmanaged
}  // namespace Midionette
}  // namespace Elgraiv

