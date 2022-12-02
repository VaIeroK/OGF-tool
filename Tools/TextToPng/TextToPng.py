from PIL import Image, ImageDraw, ImageFont
from sys import argv

# Compile:
# python -m auto_py_to_exe

def create_texture(Text: str, OutPath: str):

    ImageW, ImageH = (8, 8)
    TextW, TextH = (8, 8)
    Font = ImageFont.truetype("cour.ttf", 32, encoding = 'UTF-8')

    if Text != "":
        TempImg = Image.new("RGBA", (ImageW, ImageH), "blue")
        TempDraw = ImageDraw.Draw(TempImg)
        TextW, TextH = TempDraw.textsize(Text, font = Font)
        ImageW, ImageH = (TextW, TextW)

    Texture = Image.new("RGBA", (ImageW, ImageH), "blue")
    if Text != "":
        TextDraw = ImageDraw.Draw(Texture)
        TextDraw.text(((ImageW - TextW) / 2, (ImageH - TextH) / 2), Text, fill = "black", font = Font)
    Texture.save(OutPath)

if __name__ == '__main__':
    TexturesCount = int(argv[1])
    ArgReader = 2

    for i in range(0, TexturesCount):
        create_texture(str(argv[ArgReader]), str(argv[ArgReader + 1]))
        ArgReader = ArgReader + 2