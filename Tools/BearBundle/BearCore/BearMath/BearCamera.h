#pragma once
class BEARTOOL_API  BearCamera
{
public:
	BearCamera();
	void SetPosition(BearVector3<float> pox);
	void SetRotation(BearVector3<float> rot);
	void SetPosition(float x,float y,float z);
	void SetRotation(float x, float y, float z);
	void AddPosition(BearVector3<float> pox);
	void AddRotation(BearVector3<float> rot);
	void AddPosition(float x, float y, float z);
	void AddRotation(float x, float y, float z);
	BearVector3<float>&GetRotation();
	const BearVector3<float>&GetRotation()const;

	const BearMatrix&GetMatrix()const;
	void Render();

	void Copy(const BearCamera&right);
	void Swap(BearCamera&right);

	BearCamera(const BearCamera&right);
	BearCamera&operator=(const BearCamera&right);
	BearCamera( BearCamera&&right);
	BearCamera&operator=( BearCamera&&right);
private:
	BearMatrix matrix;
	BearVector3<float> m_position;
	BearVector3<float> m_rotation;
};