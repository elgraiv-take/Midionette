/*
 *  Created on: 2018/07/23
 *      Author: take
 */

#pragma once

#include <memory>

namespace Elgraiv {
namespace Midionette {
namespace Unmanaged {

class MidiInputCore{
public:
	MidiInputCore();
	~MidiInputCore();

private:
	class MidiImputCoreImpl;
	std::unique_ptr<MidiImputCoreImpl> m_pImpl;
};

}  // namespace Unmanaged
}  // namespace Midionette
}  // namespace Elgraiv

