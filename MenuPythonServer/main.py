from fastapi import FastAPI
from fastapi.responses import JSONResponse
from mysql.connector import connect
from dotenv import load_dotenv
import uvicorn
import os


app = FastAPI()

@app.get('/registration')
async def registration_user(login:str, pwd:str ):
    load_dotenv()
    conn = connect(host=os.getenv('DB_HOST'),password=os.getenv('DB_PASS'),user=os.getenv('DB_Uid'))

if __name__=='__main__':
    uvicorn.run(app,host='127.0.0.1',port=80)