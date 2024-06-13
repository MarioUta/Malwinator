import unittest
from function import check_file 

class TestCheckFile(unittest.TestCase):
    def test_check_file(self):
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1", "test.txt"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1", "git_profile.jpg"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1", "318334758_2131256053726309_3662636400100754418_n.jpg"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1", "fisierTest1.txt"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1", "koi.jpg"), True)

        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 2", "test.txt"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 2", "git_profile.jpg"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 2", "318334758_2131256053726309_3662636400100754418_n.jpg"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 2", "fisierTest1.txt"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 2", "koi.jpg"), True)

        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1\\Mario 3", "test.txt"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1\\Mario 3", "git_profile.jpg"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1\\Mario 3", "318334758_2131256053726309_3662636400100754418_n.jpg"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1\\Mario 3", "fisierTest1.txt"), True)
        self.assertEqual(check_file("C:\\Users\\vlads\\OneDrive\\Desktop\\Mario 1\\Mario 3", "koi.jpg"), True)



