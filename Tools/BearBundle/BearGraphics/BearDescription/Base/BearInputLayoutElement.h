#pragma once
struct BearInputLayoutElement
{
	BearInputLayoutElement(const bchar8* name, BearVertexFormat type, bsize offset, bool is_instance = false) :Type(type), Name(name), Offset(offset), SemanticIndex(0), IsInstance(is_instance) {}
	BearInputLayoutElement() :Type(BearVertexFormat::None), Offset(0), SemanticIndex(0), IsInstance(0) {}
	BearVertexFormat Type;
	BearStringConteniarAnsi Name;
	bsize Offset;
	bsize SemanticIndex;
	bool IsInstance;
	inline bool empty()const
	{
		return Type == BearVertexFormat::None;
	}
	inline bool operator ==(const BearInputLayoutElement& right) const
	{
		if (Type != right.Type)return false;
		if (Name != right.Name)return false;
		if (Offset != right.Offset)return false;
		if (SemanticIndex != right.SemanticIndex)return false;
		return IsInstance == right.IsInstance;
	}
	inline bool operator !=(const BearInputLayoutElement& right) const
	{
		if (Type != right.Type)return true;
		if (Name != right.Name)return true;
		if (Offset != right.Offset)return true;
		if (SemanticIndex != right.SemanticIndex)return true;
		return IsInstance != right.IsInstance;
	}
	inline bool operator<(const BearInputLayoutElement& right)const
	{
		if (Type != right.Type)return Type < right.Type;
		if (Name != right.Name)return Name < right.Name;
		if (Offset != right.Offset)return Offset < right.Offset;
		if (SemanticIndex != right.SemanticIndex)return SemanticIndex < right.SemanticIndex;
		return IsInstance != right.IsInstance;
	}

};