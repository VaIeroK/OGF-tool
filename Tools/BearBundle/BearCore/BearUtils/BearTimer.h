#pragma once

class BEARTOOL_API  BearTimer
{
public:
	BearTimer()
	{
		m_factor = 1.f;
		m_pause = false;
		m_timer = GetCurrentTime();
	}
	inline void restart()
	{
		if (!m_pause)
			m_timer = GetCurrentTime();
	}
	inline BearTime get_elapsed_time() const
	{
		if (m_pause)return m_pause_time;
		return  (GetCurrentTime() - m_timer)*m_factor;
	}
	inline void pause(bool paused)
	{
		if (paused)pause(); else play();
	}
	inline void pause()
	{
		m_pause = true;
		m_pause_time = (GetCurrentTime() - m_timer);
	}
	inline void play()
	{
		if (!m_pause)return;
		m_timer = GetCurrentTime() - m_pause_time;
		m_pause = false;

	}
	inline bool paused()const { return m_pause; }
	inline void set_time_factor(double factor) { m_factor = factor; }
	inline double get_time_factor()const { return m_factor; }
	static BearTime GetCurrentTime();

	BearTimer(BearTimer&&right)
	{
		m_factor = 1.f;
		m_pause = false;
		m_timer = GetCurrentTime();
		swap(right);
	}
	BearTimer(const BearTimer&right)
	{
		m_factor = 1.f;
		m_pause = false;
		m_timer = GetCurrentTime();
		copy(right);
	}
	inline 		void swap(BearTimer&right)
	{
		m_timer.swap(right.m_timer);
		bear_swap(m_pause, right.m_pause);
		m_pause_time.swap(right.m_pause_time);
		bear_swap(m_factor, right.m_factor);
	}
	inline 		void copy(const BearTimer&right)
	{
		m_timer.copy(right.m_timer);
		m_pause = right.m_pause;
		m_pause_time.copy(right.m_pause_time);
		m_factor = right.m_factor;
	}
	inline 		BearTimer &operator=(const BearTimer &right)
	{
		copy(right);
		return*this;
	}
	inline 		BearTimer &operator=(BearTimer &&right)
	{
		swap(right);
		return*this;
	}
private:

	BearTime m_timer;
	bool m_pause;
	BearTime m_pause_time;
	double m_factor;
};