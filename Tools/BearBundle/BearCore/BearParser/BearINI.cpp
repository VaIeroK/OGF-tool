#include "BearCore.hpp"

BearINI::BearINI()
{
}

BearINI::~BearINI()
{
	Clear();
}

bool BearINI::LoadFromFile(const bchar * name_ini, BearEncoding::Encoding typetext, BearIncluder*icluder)
{
	BearFileStream stream;
	if (!stream.Open(name_ini))
		return false;
	LoadFromStream(stream, typetext, icluder);
	return true;
}

void BearINI::LoadFromStream(const BearInputStream & stream, BearEncoding::Encoding typetext, BearIncluder*icluder)
{
	BearMemoryStream memory(stream);
	LoadFromBuffer(memory, typetext, icluder);
}

void BearINI::LoadFromBuffer(const BearBufferedReader & stream, BearEncoding::Encoding typetext, BearIncluder*icluder )
{
	decoder(stream, typetext, icluder);
}

bool BearINI::SaveToFile(const bchar * name_ini, BearEncoding::Encoding typetext) const
{
	BearFileStream stream;
	if (!stream.Open(name_ini,stream.M_Write))
		return false;
	coder(stream, typetext);
	return true;
}


BearINI::BearINI(const BearINI & right)
{
	Copy(right);
}

BearINI::BearINI(BearINI && right)
{
	Swap(right);
}

void BearINI::Swap(BearINI & right)
{
	sections.swap(right.sections);
}

void BearINI::Copy(const BearINI & right)
{
	sections.copy(right.sections);
}

BearINI & BearINI::operator=(const BearINI & right)
{
	Copy(right);
	return *this;
}

BearINI & BearINI::operator=(BearINI && right)
{
	Swap(right);
	return *this;
}

void BearINI::Clear()
{
	sections.clear();
}

void BearINI::decoder(const BearBufferedReader & base, BearEncoding::Encoding typetext, BearIncluder*includer)
{
	BearString str;
	auto current_section = sections.end();
	while (!base.Eof())
	{
		
		str.clear_no_free();
		base.ReadStringToLine(str, typetext);
		str.seek(0);
		str.sub_space_in_begin();
		if (str.to_char(TEXT(";")[0]))
		{
			if (str.tell() == 0)
			{
				**str = 0;
			}
			else
			{
				str--;
				if (**str != TEXT("\\")[0])
				{
					str++;
					**str = 0;
				}
			}
		}
		str.seek(0);
		str.sub_space_in_begin();
		if (str.exist_possition(0, TEXT("#include")))
		{
			str.seek(str.tell() + 9);
			str.sub_space_in_begin();
			BEAR_ASSERT(**str==TEXT("\"")[0]);
			str.sub_space_in_end();
			BEAR_ASSERT((*str)[str.size()-1] == TEXT("\"")[0]);
			(*str)[str.size() - 1] = 0;
		
			decoder(**includer->OpenAsBuffer(*str), typetext, includer);

		}
		if (!**str)
			continue;
		if (str[0] == TEXT("[")[0])
		{


			str++;
			bchar*data = *str;
			BEAR_ASSERT(str.to_char(TEXT("]")[0]));
			str.sub_space_in_begin();
			bchar *sec_name_end = *str; str.to_char(TEXT(":")[0]);
			str.sub_space_in_begin();
			str.sub_space_in_end();
			bchar *parent_name = *str;
			*sec_name_end = 0;

			auto parent = sections.end();
			if (parent_name[0])
			{
				parent_name++;
				SSection sec;
				sec.parent = parent_name;
				//BEAR_FATALERROR(parent != sections.end(), TEXT("�� ������ �������� [" BEAR_PRINT_STR_CURRENT "]"), parent_name);
				sections.insert(BearStringConteniar( data), sec);
			}
			else
			{
				sections.insert(data);
			}
			current_section = sections.find(BearStringConteniar(data));
			continue;

		}
		else
		{
			if (!*str)continue;
			BEAR_ASSERT(current_section != sections.end());
			bchar*data = *str;
			str.to_char(TEXT("=")[0]);
			bchar*key_data =(bchar*) TEXT("");
			if (**str) {
				**str = 0;
				str++;
				str.sub_space_in_begin();
				str.sub_space_in_end();
				if (**str)
					key_data = *str;

			}
			BearString::SubSpaceInEnd(key_data);
			str.seek(0);
			str.sub_space_in_begin();
			str.sub_space_in_end();
			data = *str;
			auto item =	current_section->second.keys.find(BearStringConteniar(data));
			if (item == current_section->second.keys.end())
			{
				current_section->second.keys.insert(data);
				item = current_section->second.keys.find(BearStringConteniar(data));
			}
			item->second.push_back(key_data);
		}
	}
}

void BearINI::coder(BearOutputStream & base, BearEncoding::Encoding typetext) const
{
	auto begins = sections.begin();
	auto ends = sections.end();
	BearString str;
	while (begins != ends)
	{
		str.clear_no_free();
		str.append_printf(TEXT("[" BEAR_PRINT_STR_CURRENT "]"), *begins->first);
		if(begins->second.parent.size())
			str.append_printf(TEXT(":" BEAR_PRINT_STR_CURRENT ""), *begins->second.parent);

		base.WriteStringAndNewLine(str, typetext);
		auto begink = begins->second.keys.begin();
		auto endk = begins->second.keys.end();
		while (begink != endk)
		{
			auto begind=begink->second.begin();
			auto endd = begink->second.end();
			while (begind != endd)
			{
				str.clear_no_free();
				if (begind->size())
				{
					str.append_printf(TEXT("" BEAR_PRINT_STR_CURRENT "=" BEAR_PRINT_STR_CURRENT ""), *begink->first, **begind);
					base.WriteStringAndNewLine(str, typetext);
				}
				else
				{
					str.append_printf(TEXT("" BEAR_PRINT_STR_CURRENT ""), *begink->first);
					base.WriteStringAndNewLine(str, typetext);
				}
				begind++;
			}
			begink++;
		}
		begins++;
	}

}

BearVector<BearString>& BearINI::Key(BearStringConteniar section, BearStringConteniar key, BearString && def)
{
	auto sec = sections.find(BearStringConteniar(section));
	while (sec != sections.end())
	{
		auto k = sec->second.keys.find(BearStringConteniar(key));
		if (k == sec->second.keys.end())
		{
			if (sec->second.parent.size())
				sec = sections.find(BearStringConteniar(*sec->second.parent));
			else
				break;
		}
		else
		{
			return k->second;
		}
	}
	sec = sections.find(BearStringConteniar(section));
	if (sec == sections.end())
	{
		sections.insert(section);
		sec = sections.find(BearStringConteniar(section));
	}
	sec->second.keys.insert(key);
	auto k = sec->second.keys.find(BearStringConteniar(key));
	k->second.push_back(def);
	return 	k->second;
}

BearVector<BearString>& BearINI::Key(BearStringConteniar section, BearStringConteniar key, const BearString & def)
{
	auto sec = sections.find(BearStringConteniar(section));
	while (sec == sections.end())
	{
		auto k = sec->second.keys.find(BearStringConteniar(key));
		if (k == sec->second.keys.end())
		{
			if (sec->second.parent.size())
				sec = sections.find(BearStringConteniar(*sec->second.parent));
			else
				break;
		}
		else
		{
			return k->second;
		}
	}
	sec = sections.find(section);
	if (sec == sections.end())
	{
		sections.insert(section);
		sec = sections.find(section);
	}
	sec->second.keys.insert(key);
	auto k = sec->second.keys.find(key);
	k->second.push_back(def);
	return 	k->second;
}

const BearVector<BearString>& BearINI::Key(BearStringConteniar section, BearStringConteniar key) const
{
	auto sec = sections.find(section);
	BEAR_ASSERT(sec != sections.end());
	auto k = sec->second.keys.find(key);
	BEAR_ASSERT(k != sec->second.keys.end());
	return 	k->second;
}

BearVector< BearString>& BearINI::Key(BearStringConteniar section, BearStringConteniar key)
{
	auto sec = sections.find(section);
	BEAR_ASSERT(sec != sections.end());
	auto k = sec->second.keys.find(key);
	BEAR_ASSERT(k != sec->second.keys.end());
	return 	k->second;
}

BearINI::SSection& BearINI::Section(BearStringConteniar section)
{
	auto sec = sections.find(section);
	if (sec == sections.end())
	{
		sections.insert(section);
		sec = sections.find(section);
	}
	return sec->second;
}

bool BearINI::Exist(BearStringConteniar section, BearStringConteniar key) const
{
	auto sec = sections.find(section);
	if (sec == sections.end())
		return false;
	auto k = sec->second.keys.find(key);
	if (k == sec->second.keys.end())
		return false;
	return true;
}

bool BearINI::Exist(BearStringConteniar section) const
{
	auto sec = sections.find(section);
	return (sec != sections.end());
}
