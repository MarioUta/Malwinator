import unittest
from function import check_upload 

class TestCheckFile(unittest.TestCase):
    def test_check_upload(self):
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1", "CHATGPT-usage1.png"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1", "test.txt"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1", "git_profile.jpg"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1", "318334758_2131256053726309_3662636400100754418_n.jpg"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1", "fisierTest1.txt"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1", "koi.jpg"), "File uploaded successfully!")

        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 2", "CHATGPT-usage1.png"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 2", "test.txt"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 2", "git_profile.jpg"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 2", "318334758_2131256053726309_3662636400100754418_n.jpg"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 2", "fisierTest1.txt"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 2", "koi.jpg"), "File uploaded successfully!")

        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1\Mario 3", "CHATGPT-usage1.png"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1\Mario 3", "test.txt"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1\Mario 3", "git_profile.jpg"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1\Mario 3", "318334758_2131256053726309_3662636400100754418_n.jpg"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1\Mario 3", "fisierTest1.txt"), "File uploaded successfully!")
        self.assertEqual(check_upload("C:\\Users\\vlads\OneDrive\Desktop\Mario 1\Mario 3", "koi.jpg"), "File uploaded successfully!")
     



