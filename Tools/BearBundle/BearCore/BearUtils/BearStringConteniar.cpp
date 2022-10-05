#include "BearCore.hpp"
static BearVector< Impl::BearStringConteniarData*>* LArray = 0;
void Impl::BearStringConteniarManager::Initialize()
{
	LArray = bear_new<BearVector< Impl::BearStringConteniarData*>>();
}

const Impl::BearStringConteniarData* Impl::BearStringConteniarManager::Get(const bchar8* str)
{
	bsize len = BearString::GetSize(str);
	uint32 CRC = BearCheckSum::CRC32(str, len);
	auto item = bear_lower_bound(LArray->begin(), LArray->end(), CRC, [](Impl::BearStringConteniarData* data, uint32 val)->bool {return data->CRC < val; });
	if (item == LArray->end() || (*item)->CRC != CRC)
	{
		Impl::BearStringConteniarData* data = (Impl::BearStringConteniarData*) bear_alloc<char >(len + 1 + sizeof(Impl::BearStringConteniarData));
		bear_fill(*data);
		data->CRC = CRC;
		data->Size = len;
		BearString::Copy(data->String, len + 1, str);
		LArray->insert(item, data);
		return data;
	}
	else
	{
		Impl::BearStringConteniarData* next = *item;

		while (true)
		{
			if (Equal(next, str, len))
			{
				return next;
			}
			if (next->Next)
			{
				next = next->Next;
			}
			else
			{
				Impl::BearStringConteniarData* data = (Impl::BearStringConteniarData*) bear_alloc<char >(len + 1 + sizeof(Impl::BearStringConteniarData));
				bear_fill(*data);
				data->Depth = next->Depth + 1;
				data->CRC = CRC;
				data->Size = len;
				BearString::Copy(data->String, len + 1, str);
				next->Next = data;
				return data;
			}
		}

	}
	return nullptr;
}

const Impl::BearStringConteniarData* Impl::BearStringConteniarManager::Get(const bchar16* str)
{
	bsize len = BearString::GetSize(str);
	uint32 CRC = BearCheckSum::CRC32(str, len);
	auto item = bear_lower_bound(LArray->begin(), LArray->end(), CRC, [](Impl::BearStringConteniarData* data, uint32 val)->bool {return data->CRC < val; });
	if (item == LArray->end() || (*item)->CRC != CRC)
	{
		Impl::BearStringConteniarData* data = (Impl::BearStringConteniarData*) bear_alloc<char >(len*2 + 2 + sizeof(Impl::BearStringConteniarData));
		bear_fill(*data);
		data->CRC = CRC;
		data->Size = len;
		BearString::Copy((bchar16*)data->String, len + 1, str);
		LArray->insert(item, data);
		return data;
	}
	else
	{
		Impl::BearStringConteniarData* next = *item;

		while (true)
		{
			if (Equal(next, str, len))
			{
				return next;
			}
			if (next->Next)
			{
				next = next->Next;
			}
			else
			{
				Impl::BearStringConteniarData* data = (Impl::BearStringConteniarData*) bear_alloc<char >(len * 2 + 2 + sizeof(Impl::BearStringConteniarData));
				bear_fill(*data);
				data->Depth = next->Depth + 1;
				data->CRC = CRC;
				data->Size = len;
				BearString::Copy((bchar16*)data->String, len + 1, str);
				next->Next = data;
				return data;
			}
		}

	}
	return nullptr;
}

void Impl::BearStringConteniarManager::Destroy()
{
	for (auto b = LArray->begin(), e = LArray->end(); b != e; b++)
	{
		Impl::BearStringConteniarData* data = *b;
		{
			Impl::BearStringConteniarData* next = data->Next;

			while (next)
			{
				auto temp = next;
				next = next->Next;
				bear_free(temp);

			}
		}
		bear_free(data);
	}
	bear_delete(LArray);
}

bool Impl::BearStringConteniarManager::Equal(BearStringConteniarData* data, const bchar8* str, bsize len)
{
	if (data->Size == len)
	{
		return bear_compare(data->String, str, len) == 0;
	}
	else
	{
		return false;
	}
}

bool Impl::BearStringConteniarManager::Equal(BearStringConteniarData* data, const bchar16* str, bsize len)
{
	if (data->Size == len)
	{
		return bear_compare(data->String, str, 2*len) == 0;
	}
	else
	{
		return false;
	}
}
