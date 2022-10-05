#pragma once
class BEARTOOL_API BearINI
{
public:
	BearINI();
	~BearINI();

	bool LoadFromFile(const bchar*name_ini, BearEncoding::Encoding typetext,BearIncluder*includer = &GIncluderDefault);
	void LoadFromStream(const BearInputStream&stream,  BearEncoding::Encoding typetext, BearIncluder*includer = &GIncluderDefault);
	void LoadFromBuffer(const BearBufferedReader&stream, BearEncoding::Encoding typetext, BearIncluder*includer = &GIncluderDefault);
	bool SaveToFile(const bchar*name_ini, BearEncoding::Encoding typetext)const;

	BearINI(const BearINI&right);
	BearINI(BearINI&&right);
	void Swap(BearINI&right);
	void Copy(const BearINI&right);
	BearINI&operator=(const BearINI&right);
	BearINI&operator=(BearINI&&right);
	void Clear();

	struct SSection
	{
		BearString parent;
		BearMap<BearStringConteniar, BearVector<BearString>> keys;
	};
	BearMap<BearStringConteniar, SSection> sections;

	BearVector<BearString>&Key(BearStringConteniar section, BearStringConteniar key, BearString&&def);
	BearVector<BearString>&Key(BearStringConteniar section, BearStringConteniar key, const BearString&def);
	const BearVector<BearString>&Key(BearStringConteniar section, BearStringConteniar key)const;
	BearVector<BearString>&Key(BearStringConteniar section, BearStringConteniar key);
	SSection&Section(BearStringConteniar section);

	bool Exist(BearStringConteniar section, BearStringConteniar key)const;
	bool Exist(BearStringConteniar section)const;
private:
	void decoder(const BearBufferedReader& base,  BearEncoding::Encoding typetext, BearIncluder*includer);
	void coder(BearOutputStream& base, BearEncoding::Encoding typetext) const;

};
