#include "BearCore.hpp"
BearCamera::BearCamera()
{
	SetPosition(0, 0, 0);
	SetRotation(0, 0, 0);
}
# define PI           3.14159265358979323846f /* pi */
void BearCamera::SetPosition(BearVector3<float> pox)
{
	m_position = pox;
}

void BearCamera::SetRotation(BearVector3<float> rot)
{
	m_rotation = rot;
}

void BearCamera::SetPosition(float x, float y, float z)
{
	m_position.set(x, y, z);
}

void BearCamera::SetRotation(float x, float y, float z)
{
	m_rotation.set(x, y, z);
}

void BearCamera::AddPosition(BearVector3<float> pox)
{
	m_position += pox;
}

void BearCamera::AddRotation(BearVector3<float> rot)
{
	m_rotation += rot;
	while (m_rotation.x > (PI*2.f))
		m_rotation.x =- (PI*2.f);
	while (m_rotation.y > (PI*2.f))
		m_rotation.y =- (PI*2.f);
	while (m_rotation.z > (PI*2.f))
		m_rotation.z =- (PI*2.f);

	while (m_rotation.x <-(PI*2.f))
		m_rotation.x = +(PI*2.f);
	while (m_rotation.y  <-(PI*2.f))
		m_rotation.y = +(PI*2.f);
	while (m_rotation.z  <-(PI*2.f))
		m_rotation.z = +(PI*2.f);
}

void BearCamera::AddPosition(float x, float y, float z)
{
	AddPosition(BearVector3<float>(x, y, z));
}

void BearCamera::AddRotation(float x, float y, float z)
{
	AddRotation(BearVector3<float>(x, y, z));
}

BearVector3<float>& BearCamera::GetRotation()
{
	return m_rotation;
}

const BearVector3<float>& BearCamera::GetRotation() const
{
	return m_rotation;
}

const  BearMatrix & BearCamera::GetMatrix() const
{
	return matrix;
}

void MatrixRotationYawPitchRoll(float* matrix, float yaw, float pitch, float roll)
{
	float cYaw, cPitch, cRoll, sYaw, sPitch, sRoll;


	cYaw = cosf(yaw);
	cPitch = cosf(pitch);
	cRoll = cosf(roll);

	sYaw = sinf(yaw);
	sPitch = sinf(pitch);
	sRoll = sinf(roll);

	matrix[0] = (cRoll * cYaw) + (sRoll * sPitch * sYaw);
	matrix[1] = (sRoll * cPitch);
	matrix[2] = (cRoll * -sYaw) + (sRoll * sPitch * cYaw);

	matrix[3] = (-sRoll * cYaw) + (cRoll * sPitch * sYaw);
	matrix[4] = (cRoll * cPitch);
	matrix[5] = (sRoll * sYaw) + (cRoll * sPitch * cYaw);

	matrix[6] = (cPitch * sYaw);
	matrix[7] = -sPitch;
	matrix[8] = (cPitch * cYaw);

	return;
}


void TransformCoord(BearVector3<float>& vector, float* matrix)
{
	float x, y, z;


	x = (vector.x * matrix[0]) + (vector.y * matrix[3]) + (vector.z * matrix[6]);
	y = (vector.x * matrix[1]) + (vector.y * matrix[4]) + (vector.z * matrix[7]);
	z = (vector.x * matrix[2]) + (vector.y * matrix[5]) + (vector.z * matrix[8]);

	vector.x = x;
	vector.y = y;
	vector.z = z;

	return;
}

void BearCamera::Render()
{
	BearVector3<float> up, position, lookAt;
	float yaw, pitch, roll;
	float rotationMatrix[9];


	// Setup the vector that points upwards.
	up.x = 0.0f;
	up.y = 1.0f;
	up.z = 0.0f;

	// Setup the position of the camera in the world.
	position.x = m_position.x;
	position.y = m_position.y;
	position.z = m_position.z;

	// Setup where the camera is looking by default.
	lookAt.x = 0.0f;
	lookAt.y = 0.0f;
	lookAt.z = 1.0f;

	// Set the yaw (Y axis), pitch (X axis), and roll (Z axis) rotations in radians.
	pitch = m_rotation.x /** 0.0174532925f*/;
	yaw = m_rotation.y /** 0.0174532925f*/;
	roll = m_rotation.z /** 0.0174532925f*/;

	// Create the rotation matrix from the yaw, pitch, and roll values.
	MatrixRotationYawPitchRoll(rotationMatrix, yaw, pitch, roll);

	// Transform the lookAt and up vector by the rotation matrix so the view is correctly rotated at the origin.
	TransformCoord(lookAt, rotationMatrix);
	TransformCoord(up, rotationMatrix);

	// Translate the rotated camera position to the location of the viewer.
	lookAt.x = position.x + lookAt.x;
	lookAt.y = position.y + lookAt.y;
	lookAt.z = position.z + lookAt.z;

	// Finally create the view matrix from the three updated vectors.
	matrix.BuildLookAt(position, lookAt, up);
}

void BearCamera::Copy(const BearCamera & right)
{
	m_position = right.m_position;
	m_rotation = right.m_rotation;
	matrix = right.matrix;
}

void BearCamera::Swap(BearCamera & right)
{
	m_position.swap(right.m_position);
	m_rotation.swap(right.m_rotation);
	matrix.Swap(right.matrix);
}

BearCamera::BearCamera(const BearCamera & right)
{
	Copy(right);
}

BearCamera & BearCamera::operator=(const BearCamera & right)
{
	Copy(right);
	return *this;
}

BearCamera::BearCamera(BearCamera && right)
{
	Swap(right);
}

BearCamera & BearCamera::operator=(BearCamera && right)
{
	Swap(right);
	return *this;
}
