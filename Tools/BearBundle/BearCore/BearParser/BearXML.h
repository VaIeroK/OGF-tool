#pragma once

class BEARTOOL_API BearXML
{
public:
	typedef void* BearXmlNode;
	BearXML();
	
	~BearXML();
	bool LoadFromFile(const bchar* file, BearString&value_error, BearString&out_error);
	bool LoadFromStream(const BearInputStream& file, BearEncoding::Encoding tp, BearString&value_error, BearString&out_error);
	bool LoadFromBuffrer(const BearBufferedReader&buffer, BearEncoding::Encoding tp, BearString&value_error, BearString&out_error);
	void Clear();
	BearString  Get(BearXmlNode node,const bchar*text=TEXT(""));
	BearString  GetAtribute(BearXmlNode node,const char* Atribute, const bchar*text = TEXT(""));
	BearXmlNode GetNode(const char* name);
	BearXmlNode GetNode(BearXmlNode node, const char* name);
	BearXmlNode NextNode(BearXmlNode node, const char* name);

	BearXML(const BearXML&) = delete;
	BearXML&operator =(const BearXML&) = delete;
private:
	mutable void*	m_Doc;

};
