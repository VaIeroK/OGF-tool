#pragma once
class   BearMatrix
{
public:
	BearMatrix() {}
		
	BearMatrix(BearMatrix&&mat) { Swap(mat); }
	BearMatrix(const BearMatrix&mat) { Copy(mat); }
		
	void Swap(BearMatrix&mat)
	{
		for (bsize i = 0; i < 16; i++)
			bear_swap(m_matrix[i], mat.m_matrix[i]);
	}
	void Copy(const BearMatrix&mat)
	{
		for (bsize i = 0; i < 16; i++)
			m_matrix[i] = mat.m_matrix[i];
	}
	inline BearMatrix &operator=(const BearMatrix&mat) { Copy(mat); return*this; }
	inline BearMatrix &operator=(BearMatrix&&mat) { Swap(mat); return*this; }
		
		
	inline BearMatrix& BuildIdentity()
	{
		m_matrix[0] = 1.0f;
		m_matrix[1] = 0.0f;
		m_matrix[2] = 0.0f;
		m_matrix[3] = 0.0f;

		m_matrix[4] = 0.0f;
		m_matrix[5] = 1.0f;
		m_matrix[6] = 0.0f;
		m_matrix[7] = 0.0f;

		m_matrix[8] = 0.0f;
		m_matrix[9] = 0.0f;
		m_matrix[10] = 1.0f;
		m_matrix[11] = 0.0f;

		m_matrix[12] = 0.0f;
		m_matrix[13] = 0.0f;
		m_matrix[14] = 0.0f;
		m_matrix[15] = 1.0f;
		return *this;
	}
	inline BearMatrix& BuildPerspectiveFovLH(float fieldOfView, float screenAspect, float screenNear, float screenDepth)
	{

		bear_fill(m_matrix, 16, 0);
		m_matrix[0] = 1.0f / (screenAspect * static_cast<float>(BearMath::tan(fieldOfView * 0.5f)));
		m_matrix[5] = 1.0f / static_cast<float>(BearMath::tan(fieldOfView * 0.5f));
		m_matrix[10] = screenDepth / (screenDepth - screenNear);
		m_matrix[11] = 1.0f;
		m_matrix[14] = -(screenNear * screenDepth) / (screenDepth - screenNear);
		return  *this;

	}
	inline BearMatrix& BuildPerspectiveFovRH(float fieldOfView, float screenAspect, float screenNear, float screenDepth)
	{
		bear_fill(m_matrix, 16, 0);
		m_matrix[0] = 1.0f / (screenAspect * static_cast<float>(BearMath::tan(fieldOfView * 0.5f)));
		m_matrix[5] = 1.0f / static_cast<float>(tan(fieldOfView * 0.5f));
		m_matrix[10] = screenDepth / (screenDepth - screenNear);
		m_matrix[11] = -1.0f;
		m_matrix[14] = (screenNear * screenDepth) / (screenDepth - screenNear);
		return  *this;
	}
	inline BearMatrix& BuildOrtho(float width, float height, float screenNear, float screenDepth)
	{
		bear_fill(m_matrix, 16, 0);
		m_matrix[0] = float(2) / width;
		m_matrix[5] = float(2) / height;
		m_matrix[10] = 1.f / (screenDepth - screenNear);
		m_matrix[14] = screenNear / (screenNear - screenDepth);
		m_matrix[15] = 1;
		return *this;
	}
	inline BearMatrix& BuildOrthoOffCenter(float width, float height, float screenNear, float screenDepth)
	{
		bear_fill(m_matrix, 16, 0);
		m_matrix[0] = float(2) / width;
		m_matrix[5] = -float(2) / height;
		m_matrix[10] = (1.f / (screenDepth - screenNear));
		m_matrix[12] = -1;
		m_matrix[13] = 1;
		m_matrix[14] = screenNear / (screenNear - screenDepth);
		m_matrix[15] = 1;
		return *this;
	}
	inline BearMatrix& BuildLookAt(BearVector3<float> position, BearVector3<float> lookAt, BearVector3<float> up)
	{
		BearVector3<float> vView = lookAt;
		vView -= position;
		vView.normalize();
		float t = up.x*vView.x + up.y*vView.y + up.z*vView.z;
		BearVector3<float> vUp;
		vUp.x = (vView.x*-t) + up.x;
		vUp.y = (vView.y*-t) + up.y;
		vUp.z = (vView.z*-t) + up.z;
		vUp.normalize();
		BearVector3<float> vRight;
		vRight.x = (vUp.y  * vView.z) - vUp.z  * vView.y;
		vRight.y = (vUp.z  * vView.x) - vUp.x  * vView.z;
		vRight.z = (vUp.x  * vView.y) - vUp.y  * vView.x;

		m_matrix[0] = vRight.x; //11
		m_matrix[1] = vUp.x;//12
		m_matrix[2] = vView.x;//13
		m_matrix[3] = 0.0f;//14

		m_matrix[4] = vRight.y;//21
		m_matrix[5] = vUp.y;//22
		m_matrix[6] = vView.y;//23
		m_matrix[7] = 0.0f;   //24

		m_matrix[8] = vRight.z;//31
		m_matrix[9] = vUp.z;//32
		m_matrix[10] = vView.z;//33
		m_matrix[11] = 0.0f;   //34

		m_matrix[12] = -(position.x*vRight.x + position.y*vRight.y + position.z*vRight.z);//41
		m_matrix[13] = -(position.x*vUp.x + position.y*vUp.y + position.z*vUp.z);;//42
		m_matrix[14] = -(position.x*vView.x + position.y*vView.y + position.z*vView.z);;//43
		m_matrix[15] = 1.0f;   //44}
		return*this;
	}

	inline BearMatrix operator*(const BearMatrix&matrix)const
	{
		BearMatrix result;
		multiply(result.m_matrix, m_matrix, matrix.m_matrix);
		return result;
	}
	inline BearMatrix Multiply(const BearMatrix&matrix)const
	{
		BearMatrix result;
		multiply(result.m_matrix, m_matrix, matrix.m_matrix);
		return result;
	}
	inline BearMatrix MultiplyAs4x3(const BearMatrix&matrix)const
	{
		BearMatrix result;
		multiply43(result.m_matrix_4x4, m_matrix_4x4, matrix.m_matrix_4x4);
		return result;
	}
	inline BearMatrix& Translation(float x, float y, float z)
	{
		BuildIdentity();
		m_matrix[12] = x;
		m_matrix[13] = y;
		m_matrix[14] = z;
		return*this;
	}
	inline BearMatrix& Translation(BearVector3<float> position)
	{
		return Translation(position.x, position.y, position.z);
	}
	inline BearMatrix& Scale(float x, float y, float z)
	{
		BuildIdentity();
		m_matrix_4x4[0][0] = x;
		m_matrix_4x4[1][1] = y;
		m_matrix_4x4[2][2] = z;
		return*this;
	}

	inline BearMatrix& Scale(BearVector3<float> size)
	{
		return Scale(size.x, size.y, size.z);
	}
	inline BearMatrix&	Rotate(float h, float p,float b)
	{

		float _ch, _cp, _cb, _sh, _sp, _sb, _cc, _cs, _sc, _ss;

		_sh = BearMath::sin(h); _ch = BearMath::cos(h);
		_sp = BearMath::sin(p); _cp = BearMath::cos(p);
		_sb = BearMath::sin(b); _cb = BearMath::cos(b);
		_cc = _ch * _cb; _cs = _ch * _sb; _sc = _sh * _cb; _ss = _sh * _sb;

		m_matrix_4x4[0][0] = _cc - _sp * _ss; m_matrix_4x4[0][1] = -_cp * _sb; m_matrix_4x4[0][2] = _sp * _cs + _sc; m_matrix_4x4[0][3] = 0;
		m_matrix_4x4[1][0] = _sp * _sc + _cs; m_matrix_4x4[1][1] = _cp * _cb; m_matrix_4x4[1][2] = _ss - _sp * _cc;	m_matrix_4x4[1][3] = 0;
		m_matrix_4x4[2][0] = -_cp * _sh; m_matrix_4x4[2][1] = _sp, _cp*_ch;	m_matrix_4x4[2][2] = 0;
		m_matrix_4x4[3][0] = 0; m_matrix_4x4[3][1] = 0; m_matrix_4x4[3][1] = 0; m_matrix_4x4[3][3] = 1;
		return *this;
	}

	inline const float*operator*() const
	{
		return m_matrix;
	}
	inline float Get(bsize x, bsize y)const
	{
		return m_matrix_4x4[x][y];
	}
private:
	static inline void multiply(float* result, const float* matrix1, const float* matrix2)
	{
		result[0] = (matrix1[0] * matrix2[0]) + (matrix1[1] * matrix2[4]) + (matrix1[2] * matrix2[8]) + (matrix1[3] * matrix2[12]);
		result[1] = (matrix1[0] * matrix2[1]) + (matrix1[1] * matrix2[5]) + (matrix1[2] * matrix2[9]) + (matrix1[3] * matrix2[13]);
		result[2] = (matrix1[0] * matrix2[2]) + (matrix1[1] * matrix2[6]) + (matrix1[2] * matrix2[10]) + (matrix1[3] * matrix2[14]);
		result[3] = (matrix1[0] * matrix2[3]) + (matrix1[1] * matrix2[7]) + (matrix1[2] * matrix2[11]) + (matrix1[3] * matrix2[15]);

		result[4] = (matrix1[4] * matrix2[0]) + (matrix1[5] * matrix2[4]) + (matrix1[6] * matrix2[8]) + (matrix1[7] * matrix2[12]);
		result[5] = (matrix1[4] * matrix2[1]) + (matrix1[5] * matrix2[5]) + (matrix1[6] * matrix2[9]) + (matrix1[7] * matrix2[13]);
		result[6] = (matrix1[4] * matrix2[2]) + (matrix1[5] * matrix2[6]) + (matrix1[6] * matrix2[10]) + (matrix1[7] * matrix2[14]);
		result[7] = (matrix1[4] * matrix2[3]) + (matrix1[5] * matrix2[7]) + (matrix1[6] * matrix2[11]) + (matrix1[7] * matrix2[15]);

		result[8] = (matrix1[8] * matrix2[0]) + (matrix1[9] * matrix2[4]) + (matrix1[10] * matrix2[8]) + (matrix1[11] * matrix2[12]);
		result[9] = (matrix1[8] * matrix2[1]) + (matrix1[9] * matrix2[5]) + (matrix1[10] * matrix2[9]) + (matrix1[11] * matrix2[13]);
		result[10] = (matrix1[8] * matrix2[2]) + (matrix1[9] * matrix2[6]) + (matrix1[10] * matrix2[10]) + (matrix1[11] * matrix2[14]);
		result[11] = (matrix1[8] * matrix2[3]) + (matrix1[9] * matrix2[7]) + (matrix1[10] * matrix2[11]) + (matrix1[11] * matrix2[15]);

		result[12] = (matrix1[12] * matrix2[0]) + (matrix1[13] * matrix2[4]) + (matrix1[14] * matrix2[8]) + (matrix1[15] * matrix2[12]);
		result[13] = (matrix1[12] * matrix2[1]) + (matrix1[13] * matrix2[5]) + (matrix1[14] * matrix2[9]) + (matrix1[15] * matrix2[13]);
		result[14] = (matrix1[12] * matrix2[2]) + (matrix1[13] * matrix2[6]) + (matrix1[14] * matrix2[10]) + (matrix1[15] * matrix2[14]);
		result[15] = (matrix1[12] * matrix2[3]) + (matrix1[13] * matrix2[7]) + (matrix1[14] * matrix2[11]) + (matrix1[15] * matrix2[15]);
	}
	static inline void	multiply43(float(&result)[4][4], const float(&matrix1)[4][4], const float(& matrix2)[4][4])
	{
		result[0][0] = matrix1[0][0] * matrix2[0][0] + matrix1[1][0] * matrix2[0][1] + matrix1[2][0] * matrix2[0][2];
		result[0][1] = matrix1[0][1] * matrix2[0][0] + matrix1[1][1] * matrix2[0][1] + matrix1[2][1] * matrix2[0][2];
		result[0][2] = matrix1[0][2] * matrix2[0][0] + matrix1[1][2] * matrix2[0][1] + matrix1[2][2] * matrix2[0][2];
		result[0][3] = 0;

		result[1][0] = matrix1[0][0] * matrix2[1][0] + matrix1[1][0] * matrix2[1][1] + matrix1[2][0] * matrix2[1][2];
		result[1][1] = matrix1[0][1] * matrix2[1][0] + matrix1[1][1] * matrix2[1][1] + matrix1[2][1] * matrix2[1][2];
		result[1][2] = matrix1[0][2] * matrix2[1][0] + matrix1[1][2] * matrix2[1][1] + matrix1[2][2] * matrix2[1][2];
		result[1][3] = 0;

		result[2][0] = matrix1[0][0] * matrix2[2][0] + matrix1[1][0] * matrix2[2][1] + matrix1[2][0] * matrix2[2][2];
		result[2][1] = matrix1[0][1] * matrix2[2][0] + matrix1[1][1] * matrix2[2][1] + matrix1[2][1] * matrix2[2][2];
		result[2][2] = matrix1[0][2] * matrix2[2][0] + matrix1[1][2] * matrix2[2][1] + matrix1[2][2] * matrix2[2][2];
		result[2][3] = 0;

		result[3][0] = matrix1[0][0] * matrix2[3][0] + matrix1[1][0] * matrix2[3][1] + matrix1[2][0] * matrix2[3][2] + matrix1[3][0];
		result[3][1] = matrix1[0][1] * matrix2[3][0] + matrix1[1][1] * matrix2[3][1] + matrix1[2][1] * matrix2[3][2] + matrix1[3][1];
		result[3][2] = matrix1[0][2] * matrix2[3][0] + matrix1[1][2] * matrix2[3][1] + matrix1[2][2] * matrix2[3][2] + matrix1[3][2];
		result[3][3] = 1;
	}
	union
	{
		float m_matrix[16];
		float m_matrix_4x4[4][4];
	};
};
