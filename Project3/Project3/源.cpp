#include<iostream>
#include<vector>
using namespace std;
// ����ţ�Ͳ���������ȫ������Ӧ���������������������
// ���룺
// 4
// 1 3 2 3
// �����2
// ���룺
// 6
// 3 2 1 1 2 3
// �����2
int main()
{
	int n;
	cin >> n;
	// ע����������һ��ֵ���Ǵ���Խ�������ıȽϣ�����ο�����Ľ���˼·
	vector<int> a;
	a.resize(n + 1);
	a[n] = 0;
	//��������
	int i = 0;
	for (i = 0; i < n; ++i)
		cin >> a[i];
	i = 0;
	int count = 0;
	while (i < n)
	{
		// �ǵݼ�������
		if (a[i] < a[i + 1])
		{
			while (i < n && a[i] <= a[i + 1])
				i++;
			count++;
			i++;
		}
		else if (a[i] == a[i + 1])
		{
			i++;
		}
		else // �ǵ���������
		{
			while (i < n && a[i] >= a[i + 1])
				i++;
			count++;
			i++;
		}
	}
	cout << count << endl;
	system("pause");
	return 0;
}