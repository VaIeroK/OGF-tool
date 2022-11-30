from PIL import Image, ImageDraw, ImageFont
from sys import argv

# Compile:
# python -m auto_py_to_exe

def create_texture(Text, OutPath):
    Font = ImageFont.truetype("cour.ttf", 32, encoding = 'UTF-8')

    TempImg = Image.new("RGBA", (8, 8), "blue")
    TempDraw = ImageDraw.Draw(TempImg)
    TextW, TextH = TempDraw.textsize(Text, font = Font)

    ImageW, ImageH = (TextW, TextW)

    Texture = Image.new("RGBA", (ImageW, ImageH), "blue")
    TextDraw = ImageDraw.Draw(Texture)
    TextDraw.text(((ImageW - TextW) / 2, (ImageH - TextH) / 2), Text, fill = "black", font = Font)
    Texture.save(OutPath)

if __name__ == '__main__':
    TexturesCount = int(argv[1])
    ArgReader = 2

    for i in range(0, TexturesCount):
        create_texture(argv[ArgReader], argv[ArgReader + 1])
        ArgReader = ArgReader + 2